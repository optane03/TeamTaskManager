import axios from "axios";
import { useFormik } from "formik";
import { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";

const Url = import.meta.env.VITE_BACKEND_URL;
const AdminPassword = import.meta.env.VITE_ADMIN_PASSWORD;

interface RegisterProps {
    userName: string,
    userEmail: string,
    userPassword: string,
    role: string,
    organizationId: string
}

const defaultRegisterProps: RegisterProps = {
    userName: "",
    userEmail: "",
    userPassword: "",
    role: "",
    organizationId: "Company@123"
}

const Register = () => {
    const [admin, setAdmin] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    const navigate = useNavigate();

    useEffect(() => {
        if (sessionStorage.getItem("adminVerified") === "true") {
            setAdmin(true);
        }
    }, [])

    const handleRegister = async (values: RegisterProps) => {
        console.log(values);
        const response = await axios.post(Url + "User/Register", values);

        if (response.data.statusCode === 403 || response.data.statusCode === 404 || response.data.statusCode === 400) {
            setError(response.data.message);
        }

        if (response.data.statusCode === 201) {
            setError("");
            navigate("/");
        }
    }

    const handleAdminPassword = (password: string) => {
        if (password === AdminPassword) {
            setAdmin(true);
            setError("");
            sessionStorage.setItem("adminVerified", "true");
        } else {
            setError("Incorrect Admin Password");
        }
    }

    const formik = useFormik({
        initialValues: defaultRegisterProps,
        onSubmit: (values: RegisterProps) => {
            handleRegister(values);
        }
    })

    const submitPassword = useFormik({
        initialValues: { password: "" },
        onSubmit: (password) => { handleAdminPassword(password.password) }
    })

    return (
        <div className="m-auto bg-gray-200 rounded w-[100vw] h-[100vh] px-10 py-5">

            <form onSubmit={submitPassword.handleSubmit} className={`${!admin ? '' : 'hidden'} w-[500px] m-auto mt-25 p-3 pt-7 bg-white h-[325px] rounded-lg shadow-lg`}>
                <h1 className="text-[28px] font-bold">Verify Admin Password</h1>

                <div className="mt-10 flex gap-5 items-center px-5">
                    <label className="w-[150px] text-left text-xl">Password:</label>
                    <input
                        type="text"
                        name="password"
                        value={submitPassword.values.password}
                        onChange={submitPassword.handleChange}
                        onBlur={submitPassword.handleBlur}
                        className="bg-gray-100 rounded py-2 px-4 outline-none w-full"
                    />
                </div>

                <button type="submit" className="bg-[#1A52B9] text-white font-bold text-xl py-2 px-4 rounded mt-10 w-[150px]">Submit</button>
                {error && <div className="text-red-500 font-bold mt-8 text-[14px]">! {error}</div>}
            </form>
            {!admin && <div className="text-center mt-2">Go to <a href="/" className="font-bold text-[#0B2D72]">Home</a> Page</div>}

            <form onSubmit={formik.handleSubmit} className={`${admin ? '' : 'hidden'} w-[500px] m-auto mt-15 p-3 pt-7 bg-white h-[500px] rounded-lg shadow-lg`}>
                <h1 className="text-3xl font-bold text-[#0B2D72]">Register</h1>

                <div className="mt-10 flex gap-5 items-center px-5">
                    <label className="w-[150px] text-left text-xl">User Name:</label>
                    <input
                        type="text"
                        name="userName"
                        value={formik.values.userName}
                        onChange={formik.handleChange}
                        onBlur={formik.handleBlur}
                        className="bg-gray-100 rounded py-2 px-4 outline-none w-full"
                    />
                </div>

                <div className="mt-5 flex gap-5 items-center px-5">
                    <label className="w-[150px] text-left text-xl">Email:</label>
                    <input
                        type="email"
                        name="userEmail"
                        value={formik.values.userEmail}
                        onChange={formik.handleChange}
                        onBlur={formik.handleBlur}
                        className="bg-gray-100 rounded py-2 px-4 outline-none w-full"
                    />
                </div>

                <div className="mt-5 flex gap-5 items-center px-5">
                    <label className="w-[150px] text-left text-xl">Password:</label>
                    <input
                        type="password"
                        name="userPassword"
                        value={formik.values.userPassword}
                        onChange={formik.handleChange}
                        onBlur={formik.handleBlur}
                        className="bg-gray-100 rounded py-2 px-4 outline-none w-full"
                    />
                </div>

                <div className="mt-5 flex gap-5 items-center px-5">
                    <label className="w-[150px] text-left text-xl">Role:</label>
                    <select
                        name="role"
                        value={formik.values.role}
                        onChange={formik.handleChange}
                        onBlur={formik.handleBlur}
                        className="bg-gray-100 rounded py-2 px-4 outline-none w-full"
                    >
                        <option value="">Select Role</option>
                        <option value="Admin">Admin</option>
                        <option value="Member">Member</option>
                        <option value="Teamlead">Team Leader</option>
                    </select>
                </div>

                <button type="submit" className="bg-[#0B2D72] text-white font-bold text-xl py-2 px-4 rounded mt-15 w-[150px]">Register</button>
                {error && <p className="text-red-500 mt-5 font-bold text-[14px]">{error}</p>}
            </form>
            <div className={`${admin ? '' : 'hidden'} smt-[5px] flex gap-1 justify-center items-center`}>
                Already have an account? <Link to="/login"><p className="text-[#0B2D72] font-bold cursor-pointer">Login</p></Link>
            </div>
        </div>
    )
}

export default Register;