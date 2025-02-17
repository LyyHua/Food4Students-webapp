"use server"

import { fetchWrapper } from "@/lib/fetchWrapper"
import { PagedResult, Restaurant } from "@/types"
import { revalidatePath } from "next/cache"
import { FieldValues } from "react-hook-form"

export async function getData(query: string): Promise<PagedResult<Restaurant>> {
    return await fetchWrapper.get(`search${query}`)
}

export async function updateRestaurantTest() {
    const data = {
        name: "Trà Sữa Maycha",
        address:
            "Trà Sữa MayCha là một trong những thương hiệu trà sữa “top of mind” của giới trẻ với những sản phẩm chất lượng, sáng tạo và giá cả hợp lý. Với phương châm “Hạnh phúc trong từng lần hút”, MayCha luôn không ngừng phát triển để trao tận tay khách hàng sản phẩm ngon nhất cũng như những giá trị hạnh phúc khi thưởng thức trà sữa tại MayCha.",
        phoneNumber: "0765088561",
        description: "47B Phan Huy Ích",
        logoUrl:
            "https://static.ybox.vn/2023/6/2/1686035147740-348235363_1612468545939577_6728433341266735944_n.jpg",
        bannerUrl:
            "https://mms.img.susercontent.com/vn-11134513-7ras8-m38rifvrt5nw1e@resize_ss1242x600!@crop_w1242_h600_cT",
    }
    return await fetchWrapper.put(
        "restaurants/afbee524-5972-4075-8800-7d1f9d7b0a0c",
        data
    )
}

export async function createRestaurant(data: FieldValues) {
    return await fetchWrapper.post("restaurants", data)
}

export async function changeRestaurantStatus(id: string, state: string) {
    return await fetchWrapper.patch(`restaurants?id=${id}&state=${state}`)
}

export async function getDetailedViewData(id: string): Promise<Restaurant> {
    return await fetchWrapper.get(`search/${id}`)
}

export async function updateRestaurant(data: FieldValues, id: string) {
    const res = await fetchWrapper.put(`restaurants/${id}`, data)
    revalidatePath(`/search/${id}`)
    return res
}

export async function deleteRestaurant(id: string) {
    return await fetchWrapper.del(`restaurants/${id}`)
}
