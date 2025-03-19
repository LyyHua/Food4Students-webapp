'use client'

import { useCartStore } from "@/hooks/useCartStore"
import { useEffect } from "react"
import { placeOrder } from "../../../actions/orderActions"
import { Button } from "flowbite-react"
import { FieldValues, useForm } from "react-hook-form"
import toast from "react-hot-toast"
import Input from "../../../components/Input"
import { useRouter } from "next/navigation"
import { Console } from "console"

type Props = {
    id: string
}

export default function OrderForm({id}: Props) {
    const router = useRouter()
    const items = useCartStore((state) => state.items)
    const removeItem = useCartStore((state) => state.removeItem)
    const updateItemQuantity = useCartStore((state) => state.updateItemQuantity)
    const {
        control,
        handleSubmit,
        setFocus,
        formState: { isSubmitting, isValid },
    } = useForm({
        mode: "onTouched",
    })

    useEffect(() => {
        setFocus("")
    }, [setFocus])

    async function onSubmit(data: FieldValues) {
        try {
            const orderItems = items.map((item) => ({
                FoodItemId: item.foodItemId,
                Quantity: item.quantity,
                SelectedVariations: item.selectedVariations,
            }))

            const orderPayload = {
                ShippingAddress: data.shippingAddress,
                PhoneNumber: data.phoneNumber,
                Note: data.note,
                OrderItems: orderItems,
            }

            console.log(orderPayload)
            const res = await placeOrder(id, orderPayload)
            const orderId = res.id
            if (res.error) {
                throw res.error
            }
            router.push(`/orders/details/${orderId}`)
        } catch (error: any) {
            toast.error(error.status + " " + error.message)
        }
    }
  return (
    <form
            className="max-w-3xl mx-auto pt-8 px-4 pb-32"
            onSubmit={handleSubmit(onSubmit)}
        >
            <h2 className="text-2xl font-bold mb-4">Shopping Cart</h2>
            {items.length === 0 ? (
                <p>Your cart is empty.</p>
            ) : (
                <div className="space-y-4 mb-6">
                    {items.map((item, index) => (
                        <div
                            key={index}
                            className="flex justify-between items-center border p-3 rounded"
                        >
                            <div>
                                <p className="font-bold">
                                    Food Item Name:{" "}
                                    <span className="font-normal">
                                        {item.foodName}
                                    </span>
                                </p>
                                <div className="flex items-center gap-2 mt-1">
                                    <p className="font-bold">Quantity: </p>
                                    <Button
                                        size="xs"
                                        className="w-6 h-6 p-0 text-xs"
                                        onClick={() =>
                                            updateItemQuantity(
                                                index,
                                                item.quantity - 1
                                            )
                                        }
                                    >
                                        –
                                    </Button>
                                    <span className="font-normal w-6 text-center">
                                        {item.quantity}
                                    </span>
                                    <Button
                                        size="xs"
                                        className="w-6 h-6 p-0 text-xs"
                                        onClick={() =>
                                            updateItemQuantity(
                                                index,
                                                item.quantity + 1
                                            )
                                        }
                                    >
                                        +
                                    </Button>
                                </div>
                                {item.variationsDisplay && (
                                    <p className="text-sm text-gray-600 mt-1">
                                        {item.variationsDisplay}
                                    </p>
                                )}
                            </div>
                            <Button size="xs" onClick={() => removeItem(index)}>
                                Remove
                            </Button>
                        </div>
                    ))}
                </div>
            )}

            <div className="space-y-4">
                <Input
                    label="Shipping Address"
                    name="shippingAddress"
                    control={control}
                    rules={{ required: "Shipping Address is required" }}
                />
                <Input
                    label="Phone Number"
                    name="phoneNumber"
                    control={control}
                    rules={{ required: "Phone Number is required" }}
                />
                <Input
                    label="Add a note (optional)"
                    name="note"
                    control={control}
                />
            </div>

            <div className="mt-6">
                <Button
                    isProcessing={isSubmitting}
                    disabled={items.length === 0 || !isValid}
                    type="submit"
                    outline
                    color="success"
                >
                    {isSubmitting ? "Placing Order..." : "Place Order"}
                </Button>
            </div>
        </form>
  )
}