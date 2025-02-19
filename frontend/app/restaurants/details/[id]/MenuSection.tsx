'use client';

import { useState } from "react";
import { Button, Modal } from "flowbite-react";
import { FoodCategory, FoodItem, Variation, VariationOption } from "@/types";
import Image from "next/image";
import { numberWithCommas } from "@/lib/numberWithComma";
import { useCartStore } from "@/hooks/useCartStore";

type MenuSectionProps = {
  restaurantId: string
  foodCategories: FoodCategory[]
};

export default function MenuSection({ foodCategories, restaurantId }: MenuSectionProps) {
  return (
    <div className="mt-6 space-y-8">
      {foodCategories.map((category) => (
        <div key={category.id}>
          <h3 className="text-xl font-bold pb-1 mb-4">{category.name}</h3>
          <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
            {category.foodItems.map((item) => (
              <FoodItemCard key={item.id} foodItem={item} restaurantId={restaurantId} />
            ))}
          </div>
        </div>
      ))}
    </div>
  );
}

type FoodItemCardProps = {
  foodItem: FoodItem
  restaurantId: string
};

function FoodItemCard({ foodItem, restaurantId }: FoodItemCardProps) {
  const [modalOpen, setModalOpen] = useState(false);
  // Store selections as { variationId: [selectedOptionIds] }
  const [variationSelections, setVariationSelections] = useState<Record<string, string[]>>({});
  const [error, setError] = useState("");
  const addToCart = useCartStore((state) => state.addItem);

  const handleAdd = () => {
    // If no variations exist, add item directly with empty selections.
    if (!foodItem.variations || foodItem.variations.length === 0) {
      addToCart(restaurantId, {
        foodItemId: foodItem.id,
        quantity: 1,
        selectedVariations: [],
        foodName: foodItem.name,
        foodDescription: foodItem.description,
        price: foodItem.basePrice,
        foodItemPhotoUrl: foodItem.photoUrl,
        variationsDisplay: "",
      });
      return;
    }
    // Otherwise, open the modal for option selection.
    const initSelections: Record<string, string[]> = {};
    foodItem.variations.forEach((variation) => {
      // For radio behavior (minSelect === maxSelect === 1), auto-select first option.
      if (variation.minSelect === 1 && variation.maxSelect === 1) {
        initSelections[variation.id] = [variation.variationOptions[0].id];
      } else {
        initSelections[variation.id] = [];
      }
    });
    setVariationSelections(initSelections);
    setError("");
    setModalOpen(true);
  };

  const handleOptionClick = (
    variation: Variation,
    option: VariationOption
  ) => {
    setVariationSelections((prev) => {
      const currentSelections = prev[variation.id] || [];
      // Radio behavior for maxSelect=1 (or 0 to 1)
      if ((variation.maxSelect ?? 1) === 1) {
        return { ...prev, [variation.id]: currentSelections.includes(option.id) ? [] : [option.id] };
      }
      // Checkbox behavior for maxSelect > 1:
      let newSelections: string[] = [];
      if (currentSelections.includes(option.id)) {
        newSelections = currentSelections.filter((id) => id !== option.id);
      } else {
        if (currentSelections.length < (variation.maxSelect ?? Number.POSITIVE_INFINITY)) {
          newSelections = [...currentSelections, option.id];
        } else {
          newSelections = currentSelections;
        }
      }
      return { ...prev, [variation.id]: newSelections };
    });
  };

  const isOptionDisabled = (
    variation: Variation,
    option: VariationOption
  ) => {
    const currentSelections = variationSelections[variation.id] || [];
    if ((variation.maxSelect ?? Number.POSITIVE_INFINITY) > 1 && currentSelections.length >= (variation.maxSelect ?? Number.POSITIVE_INFINITY)) {
      return !currentSelections.includes(option.id);
    }
    return false;
  };

  const handleModalAddToCart = () => {
    // Validate that each variation meets its minSelect.
    for (const variation of foodItem.variations!) {
      const selected = variationSelections[variation.id] || [];
      if (selected.length < variation.minSelect) {
        setError(`Select at least ${variation.minSelect} option(s) for ${variation.name}`);
        return;
      }
    }
    // Build the selectedVariations array.
    const selectedVariations = Object.entries(variationSelections)
      .filter(([, optionIds]) => optionIds.length > 0)
      .map(([variationId, optionIds]) => ({
        variationId,
        variationOptionIds: optionIds,
      }));
    // Build a friendly display string for UI purposes.
    const variationsDisplay = Object.entries(variationSelections)
      .map(([variationId, optionIds]) => {
        const variation = foodItem.variations.find(v => v.id === variationId);
        if (!variation) return "";
        const optionNames = variation.variationOptions
          .filter(o => optionIds.includes(o.id))
          .map(o => o.name);
        return `${variation.name}: ${optionNames.join(" ")}`;
      })
      .filter(Boolean)
      .join(", ");
      
    addToCart(restaurantId, {
      foodItemId: foodItem.id,
      quantity: 1,
      selectedVariations,
      foodName: foodItem.name,
      foodDescription: foodItem.description,
      price: foodItem.basePrice,
      foodItemPhotoUrl: foodItem.photoUrl,
      variationsDisplay,
    });
    setModalOpen(false);
  };

  return (
    <div className="border rounded-lg p-4 flex flex-col shadow-sm bg-white">
      <div className="flex gap-4">
        <Image
          src={foodItem.photoUrl}
          alt={foodItem.name}
          width={80}
          height={80}
          className="rounded object-cover"
        />
        <div className="flex flex-col flex-grow">
          <h4 className="font-bold text-lg">{foodItem.name}</h4>
          <p className="text-sm text-gray-600 line-clamp-3">{foodItem.description}</p>
        </div>
      </div>
      <div className="mt-2 flex justify-between items-center">
        <p className="font-semibold">{numberWithCommas(foodItem.basePrice)} VND</p>
        <Button onClick={handleAdd} size="xs">+</Button>
      </div>
      {/* Modal for variations */}
      {foodItem.variations && foodItem.variations.length > 0 && (
        <Modal show={modalOpen} onClose={() => setModalOpen(false)}>
          <Modal.Header>{foodItem.name} Options</Modal.Header>
          <Modal.Body>
            {foodItem.variations.map((variation: Variation) => {
              let guideline = "";
              const max = variation.maxSelect ?? Number.POSITIVE_INFINITY;
              if (variation.minSelect === 1 && max === 1) {
                guideline = "(choose 1)";
              } else if (variation.minSelect === 0 && max === 1) {
                guideline = "(choose up to 1)";
              } else if (variation.minSelect && max !== Number.POSITIVE_INFINITY) {
                guideline = `(choose at least ${variation.minSelect} and maximum of ${max})`;
              } else if (variation.minSelect && max === Number.POSITIVE_INFINITY) {
                guideline = `(choose at least ${variation.minSelect})`;
              }
              return (
                <div key={variation.id} className="mb-4">
                  <h5 className="font-semibold mb-2">
                    {variation.name}{" "}
                    {guideline && <span className="text-xs text-gray-500">{guideline}</span>}
                  </h5>
                  <div className="flex flex-wrap gap-2">
                    {variation.variationOptions.map((option: VariationOption) => (
                      <button
                        key={option.id}
                        onClick={() => handleOptionClick(variation, option)}
                        disabled={isOptionDisabled(variation, option)}
                        className={`border rounded-full px-3 py-1 text-sm cursor-pointer hover:bg-gray-100
                          ${variationSelections[variation.id]?.includes(option.id)
                            ? "bg-blue-500 text-white"
                            : "bg-white text-black"}
                          ${isOptionDisabled(variation, option) ? "opacity-50 cursor-not-allowed" : ""}`}
                      >
                        {option.name}{" "}
                        {option.priceAdjustment !== 0 && `(+${numberWithCommas(option.priceAdjustment)})`}
                      </button>
                    ))}
                  </div>
                </div>
              );
            })}
            {error && <p className="text-red-500 text-sm">{error}</p>}
          </Modal.Body>
          <Modal.Footer>
            <Button onClick={handleModalAddToCart}>Add to Cart</Button>
            <Button onClick={() => setModalOpen(false)} color="gray">Cancel</Button>
          </Modal.Footer>
        </Modal>
      )}
    </div>
  );
}

export { FoodItemCard };