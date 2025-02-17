import { Restaurant } from "@/types"
import Link from "next/link"
import Image from "next/image"

type Props = {
    restaurant: Restaurant
}

export default function OrderPlacedToast({restaurant}: Props) {
  return (
    <Link className="flex flex-col items-center" href={`/search/${restaurant.id}`}>
        <div className="flex flex-row items-center gap-2">
            <Image
                src={restaurant.logoUrl}
                alt='Image of restaurant logo'
                height={80}
                width={80}
                className="rounded-lg w-auto h-auto"
            />
            <span>New order from {restaurant.name} has been added</span>
        </div>
    </Link>
  )
}