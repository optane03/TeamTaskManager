import axios from "axios";
import { useNavigate } from "react-router-dom";

const navigate = useNavigate();

const axiosRequest = axios.create({
    baseURL: import.meta.env.VITE_BACKEND_URL,
    withCredentials: true
});


axiosRequest.interceptors.response.use(
    (response) => {
        return response;
    },
    (error) => {
        if (error.response?.status == 401) {
            localStorage.removeItem('token')
            navigate("/")
        }

        return Promise.reject(error);
    }
);

export default axiosRequest;