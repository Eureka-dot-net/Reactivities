import { Group } from "@mui/icons-material";
import { Box, AppBar, Toolbar, Typography, Container, MenuItem } from "@mui/material";
import { NavLink } from "react-router";
import MenuItemLink from "../shared/components/MenuItemLink";
import { observer } from "mobx-react-lite";
import { useAccount } from "../../lib/hooks/useAccount";
import UserMenu from "./UserMenu";

const NavBar = observer(function NavBar() {
  const { currentUser } = useAccount();
  
  return (
    <Box sx={{ flexGrow: 1 }}>
      <AppBar position="fixed" sx={{
        backgroundImage: 'linear-gradient(135deg, #182a73 0%, #218aae 69%, #20a7ac 89%)'
      }}>
        <Container maxWidth='xl'>
          <Toolbar sx={{ display: 'flex', justifyContent: 'space-between' }}>
            <Box>
              <MenuItem component={NavLink} to='/' sx={{ display: 'flex', gap: 2 }}>
                <Group fontSize="large" />
                <Typography sx={{ position: 'relative' }} variant="h4" fontWeight={'bold'} >
                  Reactivities
                </Typography>
              </MenuItem>
            </Box>
            <Box sx={{ display: "flex" }}>
              <MenuItemLink to='/activities'>
                Activities
              </MenuItemLink>
              <MenuItemLink to='/counter'>
                Counter
              </MenuItemLink>
              <MenuItemLink to='/testErrors'>
                Error
              </MenuItemLink>
            </Box>
            <Box display='flex' alignItems='center'>
              {currentUser ? (
                <UserMenu />
              ) : (
                <>
                  <MenuItemLink to='/login'>Login</MenuItemLink>
                  <MenuItemLink to='/register'>Register</MenuItemLink>
                </>
              )}
            </Box>
          </Toolbar>
        </Container>

      </AppBar>
    </Box>
  )
})

export default NavBar;