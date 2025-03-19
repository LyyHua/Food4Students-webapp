"use server"

import { fetchWrapper } from "@/lib/fetchWrapper"
import { Order } from "@/types"
import { FieldValues } from "react-hook-form"

export async function placeOrder(id: string, data: FieldValues) {
    return await fetchWrapper.post(`orders?restaurantId=${id}`, data)
}

export async function getOrder(id: string): Promise<Order> {
    return await fetchWrapper.get(`orders/${id}`)
}

export async function changeOrderStatus(id: string, state: string) {
    console.log(`orders?id=${id}&newStatus=${state}`)
    return await fetchWrapper.patch(`orders?id=${id}&newStatus=${state}`)
}

export async function getUserOrders(): Promise<Order[]> {
    try {
        // Using the correct endpoint based on the controller's route
        const orders = await fetchWrapper.get("orders/users")

        // Check if orders is an array before attempting to map
        if (!orders || !Array.isArray(orders)) {
            console.error("Expected array of orders but got:", orders)
            return []
        }

        // Enhance orders with restaurant information
        const enhancedOrders = await Promise.all(
            orders.map(async (order: Order) => {
                try {
                    // Get restaurant details for each order
                    const restaurant = await fetchWrapper.get(
                        `search/${order.restaurantId}`
                    )
                    return {
                        ...order,
                        restaurantName:
                            restaurant?.name || "Unknown Restaurant",
                        restaurantLogo: restaurant?.logoUrl || "",
                    }
                } catch (error) {
                    // If we can't get restaurant details, return order as is
                    console.error(
                        `Failed to get restaurant details for order ${order.id}:`,
                        error
                    )
                    return {
                        ...order,
                        restaurantName: "Unknown Restaurant",
                        restaurantLogo: "",
                    }
                }
            })
        )

        return enhancedOrders
    } catch (error) {
        console.error("Error fetching user orders:", error)
        return []
    }
}

export async function getRestaurantOrders(id: string): Promise<Order[]> {
    return await fetchWrapper.get(`orders?restaurantId=${id}`)
}
