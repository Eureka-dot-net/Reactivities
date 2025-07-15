import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './app/layout/styles.css'
import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';
import 'react-toastify/dist/ReactToastify.css';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import { RouterProvider } from 'react-router';
import { router } from './app/router/routes.tsx';
import { store, StoreContext } from './lib/stores/store.ts';
import { ToastContainer } from 'react-toastify';
import { LocalizationProvider } from '@mui/x-date-pickers';
import {AdapterDateFns} from '@mui/x-date-pickers/AdapterDateFnsV3'
import { enUS, enGB, he, fr, es, type Locale } from 'date-fns/locale';
import type {} from '@mui/x-date-pickers/AdapterDateFnsV3';

const localeMap: Record<string, Locale> = {
  'en-US': enUS, // MM/DD/YYYY
  'en-GB': enGB, // DD/MM/YYYY
  'he': he,
  'fr': fr,
  'es': es,
};

const getUserLocale = (): Locale => {
  const lang = navigator.language;
  return localeMap[lang] || enGB; // default to enGB for DD/MM/YYYY
};
const queryClient = new QueryClient();

createRoot(document.getElementById('root')!).render(

  <StrictMode>
    <LocalizationProvider adapterLocale={getUserLocale()} dateAdapter={AdapterDateFns}>
      <StoreContext.Provider value={store}>
        <QueryClientProvider client={queryClient}>
          <ReactQueryDevtools initialIsOpen={false} />
          <ToastContainer position='bottom-right' theme='colored' hideProgressBar />
          <RouterProvider router={router} />
        </QueryClientProvider>
      </StoreContext.Provider>
    </LocalizationProvider>
  </StrictMode>,
)
