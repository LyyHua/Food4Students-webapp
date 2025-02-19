"use client";

import { useCartStore } from "@/hooks/useCartStore";
import { usePathname, useRouter } from "next/navigation";
import { Button } from "flowbite-react";

export default function StickyCartBar() {
  const id = useCartStore((state) => state.restaurantId);
  const items = useCartStore((state) => state.items);
  const router = useRouter();
  const pathname = usePathname();
  let showButton = true;
  let showBar = false;

  // Compute total items and total price.
  // (Assumes each item has a "quantity" and "price" property; adjust as needed.)
  const totalItems = items.reduce((acc, item) => acc + item.quantity, 0);
  const totalPrice = items.reduce((acc, item) => acc + (item.price || 0) * item.quantity, 0);

  if (items.length === 0) return null; // Only show if there are items

  if (pathname === `/orders/cart/${id}`) showButton = false;

  const handleClick = () => {
    router.push(`/orders/cart/${id}`);
  };

  const buttonText = pathname === "/orders/cart" ? "Go to Checkout" : "View Cart / Checkout";

  if (pathname === `/orders/cart/${id}` || pathname === `/restaurants/details/${id}`) showBar = true;

  return (
    <>
      {showBar &&
      <div
        className="fixed bottom-0 w-full bg-white shadow-inner p-4 flex justify-between items-center z-50 transition-all duration-300"
      >
        <div>
          <p className="font-semibold">Cart: {totalItems} items</p>
          <p className="text-sm text-gray-600">
            Total: {totalPrice.toLocaleString()} VND
          </p>
        </div>
        {showButton ? <Button onClick={handleClick}>{buttonText}</Button> : null}
      </div>
      }
    </>
  );
}