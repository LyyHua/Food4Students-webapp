import { Restaurant } from "@/types"
import Link from "next/link"
import RestaurantLogo from "./RestaurantLogo"
import { MdFavoriteBorder } from "react-icons/md"
import { FaStar } from "react-icons/fa";


type Props = {
    restaurant: Restaurant
}

export default function RestaurantCard({restaurant}: Props) {
  return (
    <Link href={`/restaurants/details/${restaurant.id}`}>
      <div className="relative w-full bg-gray-200 aspect-[16/10] rounded-lg overflow-hidden">
        <RestaurantLogo restaurant={restaurant}/>
        <div className="absolute top-2 right-2">
          <MdFavoriteBorder />
        </div>
      </div>
      <div className="flex items-center mt-4">
        <FaStar/>
        <h3 className="text-gray-700">{restaurant.averageRating} ({restaurant.totalRating})</h3>
      </div>
    </Link>
  )
}