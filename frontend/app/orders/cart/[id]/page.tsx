import OrderForm from "./OrderForm"

export default async function Page({
    params,
}: {
    params: Promise<{ id: string }>
}) {
    const id = (await params).id

    return (
        <OrderForm id={id}/>
    )
}
