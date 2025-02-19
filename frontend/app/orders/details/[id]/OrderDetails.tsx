'use client'

import { changeOrderStatus, getOrder } from "@/app/actions/orderActions";
import { Order, OrderStatus } from "@/types";
import { Button } from "flowbite-react";
import { useEffect, useState } from "react";
import toast from "react-hot-toast";

type Props = {
    id: string
    username: string
}

export default function OrderDetails({id, username}: Props) {
    const [order, setOrder] = useState<Order | null>(null);
  
    useEffect(() => {
      async function fetchOrder() {
        const data = await getOrder(id);
        setOrder(data);
      }
      fetchOrder();
    }, [id]);
  
    if (!order) return <div>Loading order details...</div>;
  
    // Determine role.
    // If the current user's username equals the order Orderer then it's the orderer side.
    // Otherwise, assume it's the restaurant owner side.
    const isOwner = username !== order.orderer;
  
    // Function to update state.
    async function handleChangeStatus(newState: string) {
      const res = await changeOrderStatus(id, newState);
      if (res.error) {
        toast.error("Status change failed: " + res.error.message);
      } else {
        toast.success("Status updated");
        // Refresh the order details
        const updated = await getOrder(id);
        setOrder(updated);
      }
    }
  
    let actionButton = null;
    if (isOwner) {
      if (order.orderStatus === 0) {
        actionButton = (
          <Button onClick={() => handleChangeStatus("Accepted")}>
            Accept Order
          </Button>
        );
      } else if (order.orderStatus === 1) {
        actionButton = (
          <Button onClick={() => handleChangeStatus("Delivering")}>
            Deliver Order
          </Button>
        );
      }
    } else {
      if (order.orderStatus === 2) {
        actionButton = (
          <Button onClick={() => handleChangeStatus("Finished")}>
            Finish Order
          </Button>
        );
      }
    }
  return (
    <div className="max-w-3xl mx-auto p-4">
        <h1 className="text-2xl font-bold mb-4">Order Details</h1>
        <div className="border p-4 rounded mb-4">
          <p><strong>Order ID:</strong> {order.id}</p>
          <p><strong>Status:</strong> {OrderStatus[order.orderStatus]}</p>
          <p><strong>Shipping Address:</strong> {order.shippingAddress}</p>
          <p><strong>Phone Number:</strong> {order.phoneNumber}</p>
          <p><strong>Note:</strong> {order.note || "None"}</p>
          <p><strong>Total Price:</strong> {order.totalPrice.toLocaleString()} VND</p>
        </div>
        <div className="mb-4">
          <h2 className="text-xl font-bold mb-2">Order Items</h2>
          {order.orderItems.map((item, index) => (
            <div key={index} className="border p-2 rounded mb-2">
              <p>
                <strong>{item.foodName}</strong> (x{item.quantity})
              </p>
              {item.variations && (
                <p className="text-sm text-gray-600">{item.variations}</p>
              )}
            </div>
          ))}
        </div>
        {actionButton && <div className="mt-4">{actionButton}</div>}
      </div>
  )
}