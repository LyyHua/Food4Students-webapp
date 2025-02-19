'use client';

import { Restaurant } from "@/types";
import {Table} from "flowbite-react";

type Props = {
    restaurant: Restaurant
}
export default function DetailedSpecs({restaurant}: Props) {
    return (
        <Table striped={true}>
            <Table.Body className="divide-y">
                <Table.Row className="bg-white dark:border-gray-700 dark:bg-gray-800">
                    <Table.Cell className="whitespace-nowrap font-medium text-gray-900 dark:text-white">
                        Owner
                    </Table.Cell>
                    <Table.Cell>
                        {restaurant.owner}
                    </Table.Cell>
                </Table.Row>
                <Table.Row className="bg-white dark:border-gray-700 dark:bg-gray-800">
                    <Table.Cell className="whitespace-nowrap font-medium text-gray-900 dark:text-white">
                        Address
                    </Table.Cell>
                    <Table.Cell>
                        {restaurant.address}
                    </Table.Cell>
                </Table.Row>
                <Table.Row className="bg-white dark:border-gray-700 dark:bg-gray-800">
                    <Table.Cell className="whitespace-nowrap font-medium text-gray-900 dark:text-white">
                        PhoneNumber
                    </Table.Cell>
                    <Table.Cell>
                        {restaurant.phoneNumber}
                    </Table.Cell>
                </Table.Row>
                <Table.Row className="bg-white dark:border-gray-700 dark:bg-gray-800">
                    <Table.Cell className="whitespace-nowrap font-medium text-gray-900 dark:text-white">
                        Total Ratings
                    </Table.Cell>
                    <Table.Cell>
                        {restaurant.totalRating}
                    </Table.Cell>
                </Table.Row>
                <Table.Row className="bg-white dark:border-gray-700 dark:bg-gray-800">
                    <Table.Cell className="whitespace-nowrap font-medium text-gray-900 dark:text-white">
                        Average Rating
                    </Table.Cell>
                    <Table.Cell>
                        {restaurant.averageRating}
                    </Table.Cell>
                </Table.Row>
                <Table.Row className="bg-white dark:border-gray-700 dark:bg-gray-800">
                    <Table.Cell className="whitespace-nowrap font-medium text-gray-900 dark:text-white">
                        Status ?
                    </Table.Cell>
                    <Table.Cell>
                        {restaurant.status}
                    </Table.Cell>
                </Table.Row>
            </Table.Body>
        </Table>
    );
}