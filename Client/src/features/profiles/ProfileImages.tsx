import { useParams } from "react-router"
import { useProfile } from "../../lib/hooks/useProfile";
import { Box, Button, Divider, ImageList, ImageListItem, Typography } from "@mui/material";
import { useState } from "react";
import ImageUploadWidget from "../../app/shared/components/ImageUploadWidget";
import StarButton from "../../app/shared/components/StarButton";
import DeleteButton from "../../app/shared/components/DeleteButton";

export default function ProfileImages() {
    const { id } = useParams();
    const { images, loadingImages, isCurrentUser, uploadImage,
        profile, setMainImage, deleteImage } = useProfile(id);
    const [editMode, setEditMode] = useState(false);

    const handleImageUpload = (file: Blob) => {
        uploadImage.mutate(file, {
            onSuccess: () => {
                setEditMode(false);
            }
        })
    }

    if (loadingImages) return <Typography>Loading...</Typography>

    if (!images) return <Typography>No images found for this user</Typography>
    return (
        <Box>

            <Box display='flex' justifyContent='space-between'>
                <Typography variant="h5">Images</Typography>
                {isCurrentUser && (
                    <Button onClick={() => setEditMode(!editMode)}>
                        {editMode ? 'Cancel' : 'Add image'}
                    </Button>
                )}
            </Box>
            <Divider sx={{ my: 2 }} />

            {editMode ? (
                <ImageUploadWidget uploadImage={handleImageUpload} loading={uploadImage.isPending} />
            ) : (
                <>
                    {images.length === 0 ? (
                        <Typography>No images added yet</Typography>
                    ) : (
                        <ImageList sx={{ height: 450 }} cols={6} rowHeight={164}>
                            {images.map((item) => (
                                <ImageListItem key={item.id}>
                                    <img
                                        srcSet={`${item.url.replace(
                                            '/upload/', '/upload/w_164,h_164,c_fill,f_auto,dpr_2,g_face/'
                                        )}`}
                                        src={`${item.url.replace(
                                            '/upload/', '/upload/w_164,h_164,c_fill,f_auto,g_face/'
                                        )}`}
                                        alt={'user profile image'}
                                        loading="lazy"
                                    />
                                    {isCurrentUser && (
                                        <div>
                                            <Box
                                                sx={{ position: 'absolute', top: 0, left: 0 }}
                                                onClick={() => setMainImage.mutate(item)}
                                            >
                                                <StarButton selected={item.url === profile?.imageUrl} />
                                            </Box>
                                            {profile?.imageUrl != item.url && (
                                                <Box
                                                    sx={{ position: 'absolute', top: 0, right: 0 }}
                                                    onClick={() => deleteImage.mutate(item.id)}
                                                >
                                                    <DeleteButton />
                                                </Box>
                                            )}
                                        </div>
                                    )}
                                </ImageListItem>
                            ))}
                        </ImageList>
                    )}
                </>
            )}
        </Box>

    )
}