"use client"

import { getUserOrders } from "../actions/orderActions"
import { Order, OrderStatus } from "@/types"
import Link from "next/link"
import { useEffect, useState } from "react"
import { Badge, Button, Spinner } from "flowbite-react"
import Image from "next/image"
import {
    FaArrowRight,
    FaClock,
    FaCheck,
    FaTruck,
    FaHourglassHalf,
} from "react-icons/fa"
import { formatDistanceToNow } from "date-fns"

export default function OrderList() {
    const [orders, setOrders] = useState<Order[]>([])
    const [loading, setLoading] = useState(true)

    useEffect(() => {
        const fetchOrders = async () => {
            const data = await getUserOrders()
            setOrders(data || [])
            setLoading(false)
        }

        fetchOrders()
    }, [])

    if (loading) {
        return (
            <div className="flex justify-center items-center h-64">
                <Spinner size="xl" />
            </div>
        )
    }

    if (orders.length === 0) {
        return (
            <div className="max-w-4xl mx-auto px-4 py-8">
                <h1 className="text-3xl font-bold mb-6">My Orders</h1>
                <div className="bg-white rounded-lg shadow-md p-8 text-center">
                    <h2 className="text-xl font-semibold text-gray-700">
                        You have not placed any orders yet
                    </h2>
                    <p className="mt-2 text-gray-500">
                        Browse restaurants and place your first order
                    </p>
                    <Link href="/search">
                        <Button color="blue" className="mt-4">
                            Find Restaurants
                        </Button>
                    </Link>
                </div>
            </div>
        )
    }

    const getStatusBadge = (status: number) => {
        switch (status) {
            case 0: // Pending
                return (
                    <Badge
                        color="gray"
                        className="flex items-center gap-1 px-2.5 py-1"
                    >
                        <div className="flex items-center">
                            <FaClock className="mr-1" /> Pending
                        </div>
                    </Badge>
                )
            case 1: // Accepted
                return (
                    <Badge
                        color="blue"
                        className="flex items-center gap-1 px-2.5 py-1"
                    >
                        <div className="flex items-center">
                            <FaHourglassHalf className="mr-1" /> Preparing
                        </div>
                    </Badge>
                )
            case 2: // Delivering
                return (
                    <Badge
                        color="yellow"
                        className="flex items-center gap-1 px-2.5 py-1"
                    >
                        <div className="flex items-center">
                            <FaTruck className="mr-1" /> On the way
                        </div>
                    </Badge>
                )
            case 3: // Finished
                return (
                    <Badge
                        color="green"
                        className="flex items-center gap-1 px-2.5 py-1"
                    >
                        <div className="flex items-center">
                            <FaCheck className="mr-1" /> Completed
                        </div>
                    </Badge>
                )
            case 4: // Cancelled
                return (
                    <Badge color="red" className="px-2.5 py-1">
                        Cancelled
                    </Badge>
                )
            default:
                return (
                    <Badge color="gray" className="px-2.5 py-1">
                        Unknown
                    </Badge>
                )
        }
    }

    // Group orders by date
    const groupedOrders: { [key: string]: Order[] } = {}
    orders.forEach((order) => {
        const date = new Date(order.createdAt).toLocaleDateString()
        if (!groupedOrders[date]) {
            groupedOrders[date] = []
        }
        groupedOrders[date].push(order)
    })

    // Helper function to group order items by foodId and include variations
    const groupOrderItems = (order: Order) => {
        // Instead of hiding details behind "variations" text, let's display them properly
        return order.orderItems.map((item) => {
            let displayText = `${item.quantity} × ${item.foodName}`

            // Add variation details if they exist
            if (typeof item.variations === "string" && item.variations) {
                displayText += ` (${item.variations})`
            } else if (
                Array.isArray(item.variations) &&
                item.variations.length > 0
            ) {
                const variationText = item.variations
                    .map((v: any) => v.name)
                    .join(", ")
                if (variationText) displayText += ` (${variationText})`
            }

            return {
                id: item.foodItemId,
                displayText,
            }
        })
    }

    return (
        <div className="max-w-4xl mx-auto px-4 py-8 mb-20">
            <h1 className="text-3xl font-bold mb-6">My Orders</h1>

            {Object.entries(groupedOrders).map(([date, dateOrders]) => (
                <div key={date} className="mb-8">
                    <h2 className="text-lg font-semibold text-gray-700 mb-3 border-b pb-2">
                        {date}
                    </h2>

                    <div className="space-y-4">
                        {dateOrders.map((order) => (
                            <div
                                key={order.id}
                                className="bg-white rounded-lg shadow-md overflow-hidden hover:shadow-lg transition-shadow"
                            >
                                <div className="p-4 border-b border-gray-100 flex justify-between items-center">
                                    <div className="flex items-center space-x-4">
                                        {order.restaurantLogo ? (
                                            <Image
                                                src={order.restaurantLogo}
                                                alt={
                                                    order.restaurantName ||
                                                    "Restaurant"
                                                }
                                                width={48}
                                                height={48}
                                                className="rounded-full border object-cover"
                                            />
                                        ) : (
                                            <div className="w-12 h-12 bg-gray-200 rounded-full flex items-center justify-center">
                                                <span className="text-gray-500 text-xs">
                                                    No logo
                                                </span>
                                            </div>
                                        )}
                                        <div>
                                            <h3 className="font-medium text-lg">
                                                {order.restaurantName ||
                                                    `Restaurant #${order.restaurantId.substring(
                                                        0,
                                                        8
                                                    )}`}
                                            </h3>
                                            <p className="text-sm text-gray-500">
                                                {formatDistanceToNow(
                                                    new Date(order.createdAt),
                                                    { addSuffix: true }
                                                )}
                                            </p>
                                        </div>
                                    </div>
                                    {getStatusBadge(order.orderStatus)}
                                </div>

                                <div className="p-4">
                                    <div className="flex flex-wrap gap-2 mb-3">
                                        {groupOrderItems(order).map(
                                            (item, idx) => (
                                                <div
                                                    key={idx}
                                                    className="bg-gray-50 px-3 py-1 rounded-full text-sm"
                                                >
                                                    {item.displayText}
                                                </div>
                                            )
                                        )}
                                    </div>

                                    <div className="flex justify-between items-center mt-4">
                                        <div className="text-lg font-semibold">
                                            {order.totalPrice.toLocaleString()}{" "}
                                            VND
                                        </div>
                                        <Link
                                            href={`/orders/details/${order.id}`}
                                        >
                                            <Button
                                                color="light"
                                                className="flex items-center"
                                            >
                                                <span className="inline-flex items-center">
                                                    View Details
                                                    <FaArrowRight
                                                        size={14}
                                                        className="ml-1 align-text-bottom"
                                                        style={{
                                                            marginTop: "1px",
                                                        }}
                                                    />
                                                </span>
                                            </Button>
                                        </Link>
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            ))}
        </div>
    )
}
