
import { Route, Routes } from 'react-router-dom'
import './App.css'
import Login from './Pages/Login'
import Register from './Pages/Register'
import WebsiteFrontPage from './Pages/WebsiteFronPage'
import AdminDashBoard from './Pages/AdminDashBoard'
import UserDashBoard from './Pages/UserDashBoard'
import ProtectedRoute from './Utils/ProtectedRoute'

function App() {
  return (
    <>
      <Routes>
        <Route path="/" element={<WebsiteFrontPage />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />

        <Route
          path='/adminDashBoard'
          element={<ProtectedRoute allowedRoles={['Admin']}>
            <AdminDashBoard />
          </ProtectedRoute  >}
        />

        <Route
          path='/userDashBoard'
          element={<ProtectedRoute allowedRoles={['Member']}>
            <UserDashBoard />
          </ProtectedRoute>}
        />
        
      </Routes>
    </>
  )
}

export default App
