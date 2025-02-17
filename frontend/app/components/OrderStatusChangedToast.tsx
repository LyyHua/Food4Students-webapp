import { Order } from "@/types"

type Props = {
    order: Order
}

export default function OrderStatusChangedToast({order}: Props) {
  return (
    <div>{order.id}</div>
  )
}