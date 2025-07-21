import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import agent from "../api/agent"
import { useMemo, useState } from "react";
import type { ProfileSchema } from "../schemas/profileSchema";

export const useProfile = (id?: string, predicate?: string) => {
    const [filter, setFilter] = useState<string>('future');
    const queryClient = useQueryClient();

    const { data: profile, isLoading: loadingProfile } = useQuery<Profile>({
        queryKey: ['profile', id],
        queryFn: async () => {
            const response = await agent.get<Profile>(`/profiles/${id}`);
            return response.data;
        },
        enabled: !!id && !predicate
    })

    const { data: userActivities, isLoading: loadingUserActivities } = useQuery<UserActivity[]>({
        queryKey: ['userActivities', id, filter],
        queryFn: async () => {
            const response = await agent.get<UserActivity[]>(`/profiles/${id}/activities`, {
                params: { filter }
            });
            return response.data;
        },
        enabled: !!id && !!filter
    });

    const editProfile = useMutation({
        mutationFn: async (profile: ProfileSchema) => {
            await agent.put('/profiles', profile);
        },
        onSuccess: (_, profile) => {
            queryClient.setQueryData(['profile', id], (data: Profile | undefined) => {
                if (!data) return data
                return {
                    ...data,
                    displayName: profile.displayName,
                    bio: profile.bio
                }
            });
            queryClient.setQueryData(['user', id], (data: User | undefined) => {
                if (!data) return data;
                return {
                    ...data,
                    displayName: profile.displayName
                }
            })
        }
    })

    const { data: images, isLoading: loadingImages } = useQuery<Image[]>({
        queryKey: ['images', id],
        queryFn: async () => {
            const response = await agent.get<Image[]>(`/profiles/${id}/images`);
            return response.data;
        },
        enabled: !!id && !predicate
    });

    const { data: followings, isLoading: loadingFollowings } = useQuery<Profile[]>({
        queryKey: ['following', id, predicate],
        queryFn: async () => {
            const response = await agent.get<Profile[]>(`/profiles/${id}/follow-list/?predicate=${predicate}`);
            return response.data;
        },
        enabled: !!id && !!predicate
    });

    const uploadImage = useMutation({
        mutationFn: async (file: Blob) => {
            const formData = new FormData();
            formData.append('file', file);
            const response = await agent.post('/profiles/add-image', formData, {
                headers: { 'Content-Type': 'multipart/form-data' }
            })
            return response.data;
        },
        onSuccess: async (image: Image) => {
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
            queryClient.setQueryData(['user'], (userData: User) => {
                if (!userData) return userData
                return {
                    ...userData,
                    imageUrl: image.url
                }
            });
            queryClient.setQueryData(['profile', id], (data: Profile) => {
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

    const updateFollowing = useMutation({
        mutationFn: async () => {
            await agent.post(`/profiles/${id}/follow`)
        },
        onSuccess: () => {
            queryClient.setQueryData(['profile', id], (profile: Profile) => {
                queryClient.invalidateQueries({ queryKey: ['following', id, 'followers'] });
                if (!profile || profile.followersCount === undefined) return profile;
                return {
                    ...profile,
                    isFollowing: !profile.isFollowing,
                    followersCount: profile.isFollowing
                        ? profile.followersCount - 1
                        : profile.followersCount + 1
                }
            });

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
        deleteImage,
        editProfile,
        updateFollowing,
        followings,
        loadingFollowings,
        userActivities,
        loadingUserActivities,
        setFilter,
        filter
    }
}