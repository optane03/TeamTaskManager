import { Link } from "react-router-dom";
import picture from "../Pictures/ChatGPT Image Apr 1, 2026, 12_22_58 AM.png"

const WebsiteFrontPage = () => {
    return (
        <div className="w-full h-full px-5 sm:px-15 py-5 font-serif">
            <div className="flex justify-between items-center w-full h-[50px]">
                <h1 className="text-2xl font-bold text-[#0B2D72]">Team Task Manager</h1>
                <div className="flex gap-3 text-[18px]">
                    <Link to="/login">
                        <button className="bg-[#0B2D72] text-white px-4 py-2 rounded-[15px]">Login</button>
                    </Link>

                    <Link to="/register">
                        <button className="bg-[#0B2D72] text-white px-4 py-2 rounded-[15px]">Register</button>
                    </Link>
                </div>
            </div>

            <div className="flex flex-col lg:flex-row  items-center justify-center h-full w-full">
                <div className="w-full h-full p-10">
                    <div className="text-[32px] font-bold">
                        Manage Tasks. 
                        Deliver Faster.
                    </div>
                    <div className="mt-5 text-[18px]">
                        A powerful team task management system designed to organize projects, assign responsibilities, and track progress efficiently—all in one place.
                    </div>
                </div>

                <img src={picture} className="w-[900px] h-auto" />
            </div>
        </div>
    )
}

export default WebsiteFrontPage;