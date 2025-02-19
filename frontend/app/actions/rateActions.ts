"use server"

import { fetchWrapper } from "@/lib/fetchWrapper"
import { PagedResult, Rating } from "@/types"
import { FieldValues } from "react-hook-form"

export async function getRatingsForRestaurant(
    id: string
): Promise<PagedResult<Rating[]>> {
    return await fetchWrapper.get(`ratings/${id}`)
}

export async function rateRestaurant(data: FieldValues) {
    return await fetchWrapper.post("ratings", data)
}
