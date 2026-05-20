import axios from "axios";
import { useFormik } from "formik";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

const Url = import.meta.env.VITE_BACKEND_URL;

interface LoginProps {
    email: string,
    password: string
}

const defaultLoginProps: LoginProps = {
    email: "",
    password: ""
}

const Login = () => {
    const [error, setError] = useState<string | null>(null);

    const navigate = useNavigate();

    const HandleLogin = async (values: LoginProps) => {

        const response = await axios.post(Url + "User/Login", values);

        if (response.data.statusCode === 404 || response.data.statusCode === 400) {
            setError(response.data.message);
        }

        if (response.data.statusCode === 200) {

            setError("");

            const token = response.data.data.token;

            sessionStorage.setItem("token", token);

            const payload = JSON.parse(atob(token.split(".")[1]));
            
            const role =
                payload[
                "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                ];

            if (role === "Admin") {
                navigate("/adminDashBoard");
            } else {
                navigate("/userDashBoard");
            }
        }
    }

    const formik = useFormik({
        initialValues: defaultLoginProps,
        onSubmit: (values: LoginProps) => {
            HandleLogin(values);
        }
    })

    return (
        <div className="m-auto bg-gray-200 rounded w-[100vw] h-[100vh] px-10 py-5 ">
            <form onSubmit={formik.handleSubmit} className="w-[450px] m-auto mt-20 p-3 pt-10 bg-white h-[400px] rounded-lg shadow-lg ">
                <h1 className="text-3xl font-bold text-[#0B2D72]">Login</h1>
                <div className="mt-15 flex gap-5 items-center px-5">
                    <label className="w-[150px] text-left text-xl">Email:</label>
                    <input
                        type="email"
                        name="email"
                        value={formik.values.email}
                        onChange={formik.handleChange}
                        onBlur={formik.handleBlur}
                        className="bg-gray-100 rounded py-2 px-4 outline-none w-full"
                    />
                </div>

                <div className="mt-5 flex gap-5 items-center px-5">
                    <label className="w-[150px] text-left  text-xl">Password:</label>
                    <input
                        type="password"
                        name="password"
                        value={formik.values.password}
                        onChange={formik.handleChange}
                        onBlur={formik.handleBlur}
                        className="bg-gray-100 rounded py-2 px-4 outline-none w-full"
                    />
                </div>

                <button type="submit" className="bg-[#0B2D72] text-white font-bold text-xl py-2 px-4 rounded mt-10 w-[150px]">Login</button>
                {error && <p className="text-red-500 mt-5 font-bold text-[14px]">{error}</p>}
            </form>

            <div className="mt-[5px]">
                Don't have an account? Ask your authorization. <br />
                or <br />
                Go to <a href="/" className="font-bold text-[#0B2D72]">Home</a> Page
            </div>
        </div>
    )

}

export default Login;