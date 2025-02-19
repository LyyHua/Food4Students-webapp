import { getDetailedViewData } from "@/app/actions/restaurantActions"
import Heading from "@/app/components/Heading"
import RestaurantForm from "../../RestaurantForm"

export default async function page({
    params,
}: {
    params: Promise<{ id: string }>
}) {
    const id = (await params).id
    const data = await getDetailedViewData(id)
    return (
        <div className="mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg">
            <Heading
                title="Update your auction"
                subtitle="Please update the details of your car"
            />
            <RestaurantForm restaurant={data} />
        </div>
    )
}
