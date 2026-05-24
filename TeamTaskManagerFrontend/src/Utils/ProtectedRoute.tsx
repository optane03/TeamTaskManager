import type { JSX } from "react";
import { Navigate } from "react-router-dom";

type Props = {
    children: JSX.Element;
    allowedRoles: string[];
}

export default function ProtectedRoute({ children, allowedRoles }: Props) {
    const token = sessionStorage.getItem("token"); 

    if(token == null){
        return <Navigate to="/login" />;
    }

    const payload = JSON.parse(atob(token.split(".")[1]));
    const userRole = payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

    if (allowedRoles && !allowedRoles.includes(userRole)) {
        return <Navigate to="/" />
    } 

    return children;
}
