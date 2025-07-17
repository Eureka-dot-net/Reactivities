import {  useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import agent from "../api/agent"
import { useMemo } from "react";

export const useProfile = (id?: string) => {
    const queryClient = useQueryClient();

    const {data: profile, isLoading: loadingProfile} = useQuery<Profile>({
        queryKey: ['profile', id],
        queryFn: async () => {
            const response = await agent.get<Profile>(`/profiles/${id}`);
            return response.data;
        },
        enabled: !!id
    })

     const {data: images, isLoading: loadingImages} = useQuery<Image[]>({
        queryKey: ['images', id],
        queryFn: async() => {
            const response = await agent.get<Image[]>(`/profiles/${id}/images`);
            return response.data;
        },
        enabled: !!id
    })

    const uploadImage = useMutation({
        mutationFn: async (file: Blob) => {
            const formData = new FormData();
            formData.append('file', file);
            const response = await agent.post('/profiles/add-image', formData, {
                headers: {'Content-Type': 'multipart/form-data'}
            })
            return response.data;
        },
        onSuccess: async(image: Image) => {
            await queryClient.invalidateQueries({
                queryKey: ['images', id]
            });
            queryClient.setQueryData(['user'], (data: User) => {
                if (!data) return data;
                return {
                    ...data,
                    imageUrl: data.imageUrl ?? image.url
                }
            });
            queryClient.setQueryData(['profile', id], (data: Profile) => {
                if (!data) return data;
                return {
                    ...data,
                    imageUrl: data.imageUrl ?? image.url
                }
            })
        }
    })

    const setMainImage = useMutation({
        mutationFn: async (image: Image) => {
            await agent.put(`/profiles/${image.id}/setMain`)
        },
        onSuccess: (_, image) => {
            queryClient.setQueryData(['user'], (userData : User) => {
                if (!userData) return userData
                return {
                    ...userData,
                    imageUrl: image.url
                }
            });
            queryClient.setQueryData(['profile', id], (data : Profile) => {
                if (!data) return data
                return {
                    ...data,
                    imageUrl: image.url
                }
            })
        } 
    })

    const deleteImage = useMutation({
        mutationFn: async (imageId: string) => {
            await agent.delete(`/profiles/${imageId}/images`)
        },
        onSuccess: (_, imageId) => {
            queryClient.setQueryData(['images', imageId], (images: Image[]) => {
                return images?.filter(x => x.id != imageId)
            })
        }
    })

    const isCurrentUser = useMemo(() => {
        return id === queryClient.getQueryData<User>(['user'])?.id
    }, [id, queryClient])

    return {
        profile,
        loadingProfile,
        images, 
        loadingImages,
        isCurrentUser,
        uploadImage,
        setMainImage,
        deleteImage
    }
}