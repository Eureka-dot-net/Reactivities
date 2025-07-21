import { useParams } from "react-router"
import { useProfile } from "../../lib/hooks/useProfile";
import { Box, Divider, Typography } from "@mui/material";
import ProfileCards from "./ProfileCards";

type Props = {
    predicate: string
}
export default function ProfileFollowings({ predicate }: Props) {
    const { id } = useParams();
    const { profile, followings, loadingFollowings } = useProfile(id, predicate);
    return (
        <Box>
            <Box display='flex'>
                <Typography variant="h5">
                    {predicate === "followings" ? `People ${profile?.displayName} is following`
                        : `People following ${profile?.displayName}`}
                </Typography>
            </Box>
            <Divider sx={{ my: 2 }} />
            {loadingFollowings ? <Typography>Loading....</Typography> : (
                <Box display='flex' marginTop={3} gap={3}>
                    {followings?.map(profile => (
                        <ProfileCards key={profile.id} profile={profile} />
                    ))}
                </Box>
            )}
        </Box>
    )
}