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
    return await fetchWrapper.get("orders")
}

export async function getRestaurantOrders(id: string): Promise<Order[]> {
    return await fetchWrapper.get(`orders?restaurantId=${id}`)
}
