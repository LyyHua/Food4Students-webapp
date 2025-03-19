"use client"

import OrderList from "./OrderList"
import { Suspense } from "react"
import { Spinner } from "flowbite-react"

export default function OrderPage() {
    return (
        <Suspense
            fallback={
                <div className="flex justify-center items-center h-64">
                    <Spinner size="xl" />
                </div>
            }
        >
            <OrderList />
        </Suspense>
    )
}
