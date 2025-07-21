import React, { useEffect, useState } from 'react';
import {
    Box,
    Card,
    CardContent,
    CardMedia,
    Tabs,
    Tab,
    Typography
} from '@mui/material';
import Grid2 from '@mui/material/Grid2';
import { useProfile } from '../../lib/hooks/useProfile';
import { useParams } from 'react-router';

const ProfileActivities: React.FC = () => {
    const [tabIndex, setTabIndex] = useState(0);

    const { id } = useParams();
    const { userActivities, loadingUserActivities, setFilter } = useProfile(id);
    useEffect(() => {
            setFilter('future');
    }, [setFilter]);
    const handleTabChange = (_: React.SyntheticEvent, newIndex: number) => {
        setTabIndex(newIndex);
        switch (newIndex) {
            case 0:
                setFilter('future')
                break;
            case 1:
                setFilter('past')
                break;
            case 2:
                setFilter('hosting')
                break;
        }
    };

    return (
        <Box sx={{ width: '100%', mt: 2 }}>
            <Tabs value={tabIndex} onChange={handleTabChange} >
                <Tab label="Future Activities" />
                <Tab label="Past Activities" />
                <Tab label="Hosting" />
            </Tabs>
            {loadingUserActivities ? (
                <Typography sx={{ mt: 2 }}>Loading....</Typography>
            ) :
                <Grid2
                    container
                    spacing={2}
                    sx={{ mt: 2, overflow: 'auto', height: 350 }}
                >
                    {userActivities?.map(activity => (
                        <Grid2 key={activity.id}>
                            <Card>
                                <CardMedia
                                    component="img"
                                    height="140"
                                    image={`/images/categoryImages/${activity.category}.jpg`}
                                    alt={activity.category}
                                />
                                <CardContent>
                                    <Typography variant="h6" gutterBottom>
                                        {activity.title}
                                    </Typography>
                                    <Typography variant="body2" color="text.secondary">
                                        {new Date(activity.date).toLocaleDateString(undefined, {
                                            day: '2-digit',
                                            month: 'short',
                                            year: 'numeric',
                                        })}{' '}
                                        <br />
                                        {new Date(activity.date).toLocaleTimeString(undefined, {
                                            hour: '2-digit',
                                            minute: '2-digit',
                                        })}
                                    </Typography>
                                </CardContent>
                            </Card>
                        </Grid2>
                    ))}
                </Grid2>
            }
        </Box>
    );
};

export default ProfileActivities;
