"use client"

import { Restaurant } from "@/types"
import { HiBadgeCheck, HiPhone, HiStar } from "react-icons/hi"
import { HiMapPin } from "react-icons/hi2"
import EditButton from "./EditButton"
import DeleteButton from "./DeleteButton"

type Props = { 
    restaurant: Restaurant 
    username: string | undefined
}

export default function RestaurantDetailsHeader({ restaurant, username }: Props) {
    return (
        <div className="mt-6">
            {/* Name & Description */}
            <div className="mb-4">
                <h1 className="text-3xl font-bold">{restaurant.name}</h1>
                <div className="flex items-center gap-3">
                    <p className="text-gray-600 mt-1">{restaurant.description}</p>
                    {username === restaurant.owner && (
                        <>
                            <EditButton id={restaurant.id} />
                            <DeleteButton id={restaurant.id} />
                        </>
                    )}
                </div>
            </div>

            {/* Spreading out key info in 3 equal columns */}
            <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
                <div className="flex items-center bg-yellow-50 p-4 rounded shadow">
                    <HiStar className="h-6 w-6 text-yellow-500 mr-2" />
                    <div>
                        <p className="text-lg font-bold">
                            {restaurant.averageRating}
                        </p>
                        <p className="text-sm text-gray-600">
                            ({restaurant.totalRating} ratings)
                        </p>
                    </div>
                </div>
                <div className="flex items-center bg-blue-50 p-4 rounded shadow">
                    <HiMapPin className="h-6 w-6 text-blue-500 mr-2" />
                    <p className="text-sm">{restaurant.address}</p>
                </div>
                <div className="flex items-center bg-green-50 p-4 rounded shadow">
                    <HiPhone className="h-6 w-6 text-green-500 mr-2" />
                    <p className="text-sm">{restaurant.phoneNumber}</p>
                </div>
            </div>

            {/* Status Badge */}
            <div className="mt-4">
                <span className="inline-flex items-center px-3 py-1 bg-gray-200 rounded-full text-xs font-semibold text-gray-700">
                    <HiBadgeCheck className="h-6 w-6 mr-1" />{" "}
                    {restaurant.status}
                </span>
            </div>
        </div>
    )
}
