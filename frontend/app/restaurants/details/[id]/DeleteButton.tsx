'use client'

import { deleteRestaurant } from "@/app/actions/restaurantActions"
import { Button } from "flowbite-react"
import { useRouter } from "next/navigation"
import { useState } from "react"
import toast from "react-hot-toast"

type Props = {
    id: string
}

export default function DeleteButton({id}: Props) {
    const [loading, setLoading] = useState(false)
    const router = useRouter()
    function doDelete() {
        setLoading(true)
        deleteRestaurant(id)
            .then(res => {
                if (res.error) throw res.error
                router.push('/')
            }).catch(error => {
                toast.error(error.status + ' ' + error.message)
            }).finally(() => setLoading(false))
    }
  return (
    <Button color="failure" isProcessing={loading} onClick={doDelete}>
        Delete Restaurant
    </Button>
  )
}