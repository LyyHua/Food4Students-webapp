'use client'

import { Button } from 'flowbite-react';
import { usePathname, useRouter } from 'next/navigation';
import React, { useEffect } from 'react'
import { useForm, FieldValues } from 'react-hook-form';
import Input from '../components/Input';
import { createRestaurant, updateRestaurant } from '../actions/restaurantActions';
import toast from 'react-hot-toast';
import { Restaurant } from '@/types';


type Props = {
    restaurant?: Restaurant
}

export default function RestaurantForm({ restaurant }: Props) {
    const router = useRouter();
    const pathname = usePathname();
    const { control, handleSubmit, setFocus, reset, formState: { isSubmitting, isValid } } = useForm({
        mode: 'onTouched'
    });

    useEffect(() => {
        if (restaurant) {
            const { name, address, phoneNumber, description, logoUrl, bannerUrl } = restaurant;
            reset({ name, address, phoneNumber, description, logoUrl, bannerUrl })
        }
        setFocus('name');
    }, [setFocus, restaurant, reset])

    async function onSubmit(data: FieldValues) {
        try {
            let id = '';
            let res;
            if (pathname === '/restaurants/create') {
                res = await createRestaurant(data);
                id = res.id;
            } else {
                if (restaurant) {
                    res = await updateRestaurant(data, restaurant.id);
                    id = restaurant.id;
                }
            }

            if (res.error) {
                throw res.error;
            }
            router.push(`/restaurants/details/${id}`)
        } catch (error: any) {
            toast.error(error.status + ' ' + error.message)
        }
    }

    return (
        <form className='flex flex-col mt-3' onSubmit={handleSubmit(onSubmit)}>
            <div className="grid grid-cols-2 gap-3">
                <Input label='Name' name='name' control={control} rules={{ required: 'Name is required' }} />
                <Input label='Address' name='address' control={control} rules={{ required: 'Address is required' }} />
            </div>
            <Input label='PhoneNumber' name='phoneNumber' control={control} rules={{ required: 'PhoneNumber is required' }} />
            <Input label='Description' name='description' control={control} />
            <Input label='Logo Image URL' name='logoUrl' control={control} rules={{ required: 'Logo image url is required' }} />
            <Input label='Banner Image URL' name='bannerUrl' control={control} rules={{ required: 'Banner image url is required' }} />

            <div className="flex justify-between">
                <Button outline color='gray'>Cancel</Button>
                <Button
                    isProcessing={isSubmitting}
                    disabled={!isValid}
                    outline
                    color='success'
                    type="submit">Submit</Button>
            </div>
        </form>
    )
}