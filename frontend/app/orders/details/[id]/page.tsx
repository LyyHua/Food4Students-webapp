import { getCurrentUser } from "@/app/actions/authActions";
import OrderDetails from "./OrderDetails";

export default async function OrderDetailsPage({
    params,
}: {
    params: Promise<{ id: string }>
}) {
    const id = (await params).id
    const user = await getCurrentUser()
    return (
        <>
            {user ? <OrderDetails id={id} username={user.username} /> : <div>Loading...</div>}
        </>
    )
  }