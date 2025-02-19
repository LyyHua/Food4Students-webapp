import { getCurrentUser } from "@/app/actions/authActions"
import { getDetailedViewData } from "@/app/actions/restaurantActions"
import MenuSection from "./MenuSection"
import Image from "next/image"
import RestaurantDetailsHeader from "./RestaurantDetailsHeader"
import { useCartStore } from "@/hooks/useCartStore"

export default async function Details({
    params,
}: {
    params: Promise<{ id: string }>
}) {
    const id = (await params).id
    const user = await getCurrentUser()
    const data = await getDetailedViewData(id)
    const restaurantId = data.id
    useCartStore.getState().setRestaurantId(restaurantId)

    return (
        <>
            <div className="pb-32">
                {/* Banner */}
                <div className="relative h-64 w-full">
                    <Image
                        src={data.bannerUrl}
                        alt="Banner"
                        fill
                        priority
                        className="object-cover"
                        sizes="(max-width: 768px) 100vw, (max-width: 1200px) 80vw, 70vw"
                    />
                    {/* Overlay logo */}
                    <div className="absolute bottom-[-40px] left-8">
                        <Image
                            src={data.logoUrl}
                            alt="Logo"
                            width={80}
                            height={80}
                            className="rounded-full border-4 border-white object-cover"
                        />
                    </div>
                </div>

                <div className="mt-12 px-6">
                    {/* Restaurant Header */}
                    <RestaurantDetailsHeader username={user?.username} restaurant={data} />

                    {/* Menu Section */}
                    <div className="mt-4">
                        <h2 className="text-2xl font-bold border-b">Menu</h2>
                        <MenuSection
                            foodCategories={data.foodCategories}
                            restaurantId={id}
                        />
                    </div>
                </div>
            </div>
        </>
    )
}
