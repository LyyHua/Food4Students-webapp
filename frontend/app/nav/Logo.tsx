'use client'

import { useParamsStore } from "@/hooks/useParamsStore";
import { usePathname, useRouter } from "next/navigation";
import { IoFastFoodOutline } from "react-icons/io5";

export default function Logo() {
    const router = useRouter()
    const pathname = usePathname()

    function doReset() {
        if (pathname !== "/") router.push("/")
        reset()
    }

    const reset = useParamsStore(state => state.reset)

    return (
        <div onClick={doReset} className="flex items-center gap-2 text-3xl font-semibold text-red-500 cursor-pointer">
            <IoFastFoodOutline size={34} />
            <div>Food4Students</div>
        </div>
    )
}
