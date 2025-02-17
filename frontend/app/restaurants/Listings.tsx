"use client"

import { useParamsStore } from "@/hooks/useParamsStore"
import { useRestaurantStore } from "@/hooks/useRestaurantStore"
import { useEffect, useState } from "react"
import { useShallow } from "zustand/shallow"
import queryString from "query-string"
import { getData } from "../actions/restaurantActions"
import EmptyFilter from "../components/EmptyFilter"
import RestaurantCard from "./RestaurantCard"
import AppPagination from "../components/AppPagination"
import Filters from "./Filters"

export default function Listings() {
    const [loading, setLoading] = useState(true)
    const params = useParamsStore(
        useShallow((state) => ({
            pageNumber: state.pageNumber,
            pageSize: state.pageSize,
            searchTerm: state.searchTerm,
            orderBy: state.orderBy,
            filterBy: state.filterBy,
            owner: state.owner,
        }))
    )

    const data = useRestaurantStore(
        useShallow((state) => ({
            restaurants: state.restaurants,
            totalCount: state.totalCount,
            pageCount: state.pageCount,
        }))
    )

    const setData = useRestaurantStore((state) => state.setData)
    const setParams = useParamsStore((state) => state.setParams)
    const url = queryString.stringifyUrl({ url: "", query: params })

    function setPageNumber(pageNumber: number) {
        setParams({ pageNumber })
    }

    useEffect(() => {
        getData(url).then((data) => {
            setData(data)
            setLoading(false)
        })
    }, [url, setData])

    if (loading) return <h3>Loading...</h3>

    return (
        <>
            <Filters />
            {data.totalCount === 0 ? (
                <EmptyFilter showReset />
            ) : (
                <>
                    <div className="grid grid-cols-4 gap-6">
                        {data.restaurants.map((restaurant) => (
                            <RestaurantCard key={restaurant.id} restaurant={restaurant} />
                        ))}
                    </div>
                    <div className="flex justify-center mt-4">
                        <AppPagination
                            pageChanged={setPageNumber}
                            currentPage={params.pageNumber}
                            pageCount={data.pageCount}
                        />
                    </div>
                </>
            )}
        </>
    )
}
