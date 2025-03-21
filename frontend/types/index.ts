export type PagedResult<T> = {
    results: T[]
    pageCount: number
    totalCount: number
}

export type Restaurant = {
    name: string
    address: string
    phoneNumber: string
    description: string
    logoUrl: string
    bannerUrl: string
    owner: string
    status: string
    id: string
    foodCategories: FoodCategory[]
    createdAt: string
    totalRating: number
    averageRating: number
}

export type FoodCategory = {
    id: string
    name: string
    foodItems: FoodItem[]
}

export type FoodItem = {
    id: string
    name: string
    description: string
    photoUrl: string
    basePrice: number
    variations: Variation[]
}

export type Variation = {
    id: string
    name: string
    minSelect: number
    maxSelect: number | null
    variationOptions: VariationOption[]
}

export type VariationOption = {
    id: string
    name: string
    priceAdjustment: number
}

export type Rating = {
    rating: number
    comment: string
    restaurantId: string
    userId: string
    id: string
}

export type Order = {
    orderItems: OrderItem[]
    createdAt: string
    orderStatus: OrderStatus
    shippingAddress: string
    phoneNumber: string
    name: string
    note: string
    totalPrice: number
    restaurantId: string
    id: string
    orderer: string
    restaurantName?: string  // Added for UI display
    restaurantLogo?: string  // Added for UI display
}

export type OrderItem = {
    foodName: string
    foodDescription: string
    price: number
    quantity: number
    variations?: string
    foodItemPhotoUrl: string
    foodItemId: string
    id: string
}

export type CartItem = {
    foodName: string
    foodDescription: string
    price: number
    quantity: number
    selectedVariations: {
        variationId: string
        variationOptionIds: string[]
    }[]
    foodItemPhotoUrl: string
    foodItemId: string
    variationsDisplay?: string;
}

export enum OrderStatus {
    Pending = 0,
    Accepted = 1,
    Delivering = 2,
    Finished = 3,
    Rejected = 4,
}
