import Heading from "@/app/components/Heading";
import RestaurantForm from "../RestaurantForm";

export default function page() {
  return (
    <div className="mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg">
      <Heading title="Open your own restaurant now!" subtitle="Please enter the details of your restaurant"/>
      <RestaurantForm />
    </div>
  )
}