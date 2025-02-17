"use client"

import { signOut } from "@/auth"
import { useParamsStore } from "@/hooks/useParamsStore"
import { Dropdown, DropdownDivider, DropdownItem } from "flowbite-react"
import { User } from "next-auth"
import Link from "next/link"
import { usePathname, useRouter } from "next/navigation"
import { AiOutlineLogout } from "react-icons/ai"
import { HiUser, HiCog } from "react-icons/hi"
import { IoReceiptOutline, IoRestaurantOutline } from "react-icons/io5"

type Props = {
    user: User
}

export default function UserActions({ user }: Props) {
    const router = useRouter()
    const pathname = usePathname()
    const setParams = useParamsStore((state) => state.setParams)

    function setOwner() {
        setParams({ owner: user.username })
        if (pathname !== '/') router.push('/')
    }
    return (
        <Dropdown inline label={`Welcome ${user.name}`}>
            <DropdownItem icon={HiUser} onClick={setOwner}>
                My restaurants
            </DropdownItem>
            <DropdownItem icon={IoReceiptOutline}>
                <Link href="/orders">My orders</Link>
            </DropdownItem>
            <DropdownItem icon={IoRestaurantOutline}>
                <Link href="/restaurants/create">Create your own restaurant now!!!</Link>
            </DropdownItem>
            <DropdownItem icon={HiCog}>
                <Link href="/session">Session (dev only!)</Link>
            </DropdownItem>
            <DropdownDivider />
            <DropdownItem
                icon={AiOutlineLogout}
                onClick={() => signOut({ redirectTo: "/" })}
            >
                Sign out
            </DropdownItem>
        </Dropdown>
    )
}
