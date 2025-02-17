import { Order } from "@/types"
import { create } from "zustand"

type State = {
    orders: Order[]
}

type Actions = {
    setOrders: (orders: Order[]) => void
    addOrder: (order: Order) => void
}

export const useOrderStore = create<State & Actions>((set) => ({
    orders: [],

    setOrders: (orders: Order[]) => {
        set(() => ({
            orders: orders
        }))
    },

    addOrder: (order: Order) => {
        set((state) => ({
            orders: !state.orders.find(x => x.id === order.id) ? [...state.orders, order] : [...state.orders]
        }))
    }
}))