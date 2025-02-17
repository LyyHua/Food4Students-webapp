'use client'

import { Order, Restaurant } from "@/types"
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr"
import { User } from "next-auth"
import { useParams } from "next/navigation"
import { ReactNode, useCallback, useEffect, useRef } from "react"
import toast from "react-hot-toast"
import { getOrder } from "../actions/orderActions"
import OrderPlacedToast from "../components/OrderPlacedToast"
import OrderStatusChangedToast from "../components/OrderStatusChangedToast"

type Props = {
    children: ReactNode
    user: User | null
    notifyUrl: string
}

export default function SignalRProvider({children, user, notifyUrl}: Props) {
    const connection = useRef<HubConnection | null>(null)
    const params = useParams<{id: string}>()

    const handleOrderStatusChanged = useCallback((order: Order) => {
        const foodOrder = getOrder(order.id)
        return toast.promise(foodOrder, {
            loading: 'Loading',
            success: (order: Order) => <OrderStatusChangedToast order={order} />,
            error: () => 'Order status changed'
        }, {success: {duration: 10000, icon: null}})
    }, [])

    const handleOrderPlaced = useCallback((restaurant: Restaurant) => {
        if (user?.username === restaurant.owner) {
            return toast(<OrderPlacedToast restaurant={restaurant} />, {
                duration: 10000,
            })
        }
    }, [user?.username])

    useEffect(() => {
        if (!connection.current) {
            connection.current = new HubConnectionBuilder()
                .withUrl(notifyUrl)
                .withAutomaticReconnect()
                .build()

            connection.current.start()
                .then(() => 'Connected to notification hub')
                .catch(err => console.error(err))
        }

        connection.current.on('OrderPlaced', handleOrderPlaced)
        connection.current.on('OrderStatusChanged', handleOrderStatusChanged)

        return () => {
            connection.current?.off('OrderPlaced', handleOrderPlaced)
            connection.current?.off('OrderStatusChanged', handleOrderStatusChanged)
        }

    }, [handleOrderPlaced, handleOrderStatusChanged, notifyUrl])
  return (
    children
  )
}