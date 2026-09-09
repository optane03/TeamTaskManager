import axios from "axios";

const axiosRequest = axios.create({
    baseURL: import.meta.env.VITE_BACKEND_URL,
    withCredentials: true
});


axiosRequest.interceptors.response.use(
    (response) => {
        return response;
    },
    (error) => {
        console.log(error.status    )
        if (error.response?.status == 401) {
            // window.location.href = "/"
        }

        return Promise.reject(error);
    }
);

export default axiosRequest;