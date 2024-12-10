import { Suspense } from "react"
import { BrowserRouter, Route, Routes } from "react-router-dom"

import { HomePage } from "pages/HomePage"
import { NotFoundPage } from "pages/NotFoundPage"

import { LoadingPage } from "pages/LoadingPage"

export const AppRouter = () => {
    return (
        <BrowserRouter>
            <Suspense fallback={<LoadingPage />}>
                <Routes>
                    <Route index element={<HomePage />} />

                    <Route path="*" element={<NotFoundPage />} />
                </Routes>
            </Suspense>
        </BrowserRouter>
    )
}