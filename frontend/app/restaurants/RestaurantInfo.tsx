'use client'

import { Restaurant } from "@/types";
import { Table } from "flowbite-react";

type Props = { restaurant: Restaurant };

export default function RestaurantInfo({ restaurant }: Props) {
  return (
    <div className="bg-gray-50 p-6 rounded-lg shadow-sm">
      <div className="grid grid-cols-1 sm:grid-cols-3 gap-4 text-center">
        <div>
          <p className="text-sm text-gray-600">Address</p>
          <p className="font-medium">{restaurant.address}</p>
        </div>
        <div>
          <p className="text-sm text-gray-600">Phone</p>
          <p className="font-medium">{restaurant.phoneNumber}</p>
        </div>
        <div>
          <p className="text-sm text-gray-600">Status</p>
          <p className="font-medium">{restaurant.status}</p>
        </div>
      </div>
      <div className="mt-4 grid grid-cols-2 gap-4 text-center">
        <div>
          <p className="text-sm text-gray-600">Average Rating</p>
          <p className="font-medium">{restaurant.averageRating}</p>
        </div>
        <div>
          <p className="text-sm text-gray-600">Total Ratings</p>
          <p className="font-medium">{restaurant.totalRating}</p>
        </div>
      </div>
    </div>
  );
}