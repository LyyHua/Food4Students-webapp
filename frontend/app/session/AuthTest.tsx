'use client'

import { useState } from "react"
import { Button } from "flowbite-react"
import { updateRestaurantTest } from "../actions/restaurantActions"

export default function AuthTest() {
    const [loading, setLoading] = useState(false)
    const [result, setResult] = useState<any>(null)

    function doUpdate() {
        setResult(undefined)
        setLoading(true)
        updateRestaurantTest()
            .then((res) => setResult(res))
            .catch((err) => setResult(err))
            .finally(() => setLoading(false))
    }
    return (
        <div className="flex items-center gap-4">
            <Button outline isProcessing={loading} onClick={doUpdate}>
                Test auth
            </Button>
            <div>{JSON.stringify(result, null, 2)}</div>
        </div>
    )
}
