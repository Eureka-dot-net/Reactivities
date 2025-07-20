import { Box, Button } from "@mui/material";
import { useForm } from "react-hook-form";
import { profileSchema, type ProfileSchema } from "../../lib/schemas/profileSchema";
import { zodResolver } from "@hookform/resolvers/zod";
import TextInput from "../../app/shared/components/TextInput";
import { useProfile } from "../../lib/hooks/useProfile";
import { useParams } from "react-router";
import { useEffect } from "react";

type Props = {
  onSuccess: () => void
}

export default function ProfileEditForm({onSuccess} : Props) {
  const {control, handleSubmit, reset} = useForm<ProfileSchema>({
     resolver: zodResolver(profileSchema),
      mode: 'onTouched'
  })
  const {id} = useParams();
  const {profile, editProfile} = useProfile(id);
  useEffect(() => {
  if (profile) {
    reset({
      displayName: profile.displayName,
      bio: profile.bio ?? ''
    });
  }
}, [profile, reset]);
const onSubmit= (data: ProfileSchema) => {
    editProfile.mutate(data, {
      onSuccess: () => {
        onSuccess();
      }
    })
  }
  
  return (
    <Box noValidate  component='form' display='flex' onSubmit={handleSubmit(onSubmit)} flexDirection="column" gap={3}>
      <TextInput control={control} label='Display Name' name='displayName' />
      <TextInput control={control} label='Bio' rows={5} multiline  name='bio' />
      <Button
                        type="submit"
                        color='success'
                        variant="contained"
                        disabled={editProfile.isPending}
                    >Submit</Button>
    </Box>
  )
}