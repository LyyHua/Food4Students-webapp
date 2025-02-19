import { CartItem } from "@/types"
import { create } from "zustand"

type State = {
  restaurantId: string | null;
  items: CartItem[];
};

type Actions = {
  setRestaurantId: (restaurantId: string) => void;
  addItem: (restaurantId: string, item: CartItem) => void;
  removeItem: (index: number) => void;
  updateItemQuantity: (index: number, quantity: number) => void;
  clearCart: () => void;
};

function sortSelectedVariations(arr: { variationId: string; variationOptionIds: string[] }[]) {
  return arr
    .map((v) => ({ ...v, variationOptionIds: [...v.variationOptionIds].sort() }))
    .sort((a, b) => a.variationId.localeCompare(b.variationId));
}

export const useCartStore = create<State & Actions>((set, get) => ({
  setRestaurantId: (restaurantId) => set({ restaurantId }),
  restaurantId: null,
  items: [],
  addItem: (restaurantId, newItem) => {
    // If the cart is empty, set the restaurant id.
    if (!get().restaurantId) {
      set(() => ({ restaurantId }));
    } else if (get().restaurantId !== restaurantId) {
      // Option 1: reject adding items from a different restaurant.
      // Option 2: automatically clear the cart. (Uncomment below if desired)
      // set({ restaurantId, items: [] });
      console.error("You can only order from one restaurant at a time.");
      return;
    }
    const currentItems = get().items;
    const index = currentItems.findIndex((item) => {
      return (
        item.foodItemId === newItem.foodItemId &&
        JSON.stringify(sortSelectedVariations(item.selectedVariations || [])) ===
          JSON.stringify(sortSelectedVariations(newItem.selectedVariations || []))
      );
    });
    if (index !== -1) {
      set((state) => ({
        items: state.items.map((item, idx) =>
          idx === index ? { ...item, quantity: item.quantity + newItem.quantity } : item
        ),
      }));
    } else {
      set((state) => ({
        items: [...state.items, newItem],
      }));
    }
  },
  removeItem: (index: number) => {
    set((state) => ({
      items: state.items.filter((_, idx) => idx !== index),
    }));
  },
  updateItemQuantity: (index, quantity) => {
    set((state) => ({
      items: state.items.map((item, idx) =>
        idx === index ? { ...item, quantity: quantity < 1 ? 1 : quantity } : item
      ),
    }));
  },
  clearCart: () => set({ restaurantId: null, items: [] }),
}));