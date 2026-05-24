import axios from "axios";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import dlt from "../Pictures/recycle-bin.png";
import home from "../Pictures/home-button.png";
import logout from "../Pictures/exit.png";

interface Projects {
  id: string;
  projectName: string;
  adminEmail: string;
  projectMembers: string[];
  projectStatus: string;
}

interface User {
  userName: string;
  userEmail: string;
  roll: string;
}

const AdminDashBoard = () => {
  const navigate = useNavigate();
  const [allProjects, setAllProjects] = useState<Projects[]>([]);
  const [user, setUser] = useState<User>();

  const fetchProjects = async (email: string) => {
    const projectUrl = `http://localhost:5089/api/Project/GetAllProjectDetails?email=${email}`;
    const projectResponse = await axios.get(projectUrl);

    setAllProjects(projectResponse.data.data);
    console.log(projectResponse.data.data);
  };

  const fetchUser = async (email: string) => {
    const userUrl = `http://localhost:5089/api/User/GetUser?email=${email}`;
    const userResponse = await axios.get(userUrl);

    const userData: User = {
      userName: userResponse.data.data.userName,
      userEmail: userResponse.data.data.userEmail,
      roll: userResponse.data.data.roll
    };

    setUser(userData);
  };


  useEffect(() => {
    const token = sessionStorage.getItem("token");

    if (!token) {
      navigate("/");
    }

    const payload = JSON.parse(atob(token!.split(".")[1]));

    const email = payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"];

    fetchProjects(email);
    fetchUser(email);
  }, []);


  return (
    <div className="flex w-full h-screen">
      <div className="w-1/4 bg-[#1A1953] ">
        <h1 className="text-2xl font-bold text-left px-5 py-3 m-3 text-white bg-[#141343] rounded-2xl">Dashboard</h1>
      </div>
      <div className="w-full h-full p-3 bg-blue-50">
        <div className="flex items-center justify-between">
          <h1 className="text-4xl font-bold text-left p-2 rounded-xl">Hi, {user?.userName || "User"}</h1>
          <div className="flex gap-3">
            <img src={logout} alt="Logout" onClick={() => { sessionStorage.removeItem('token'); navigate("/"); }} className="cursor-pointer h-10 w-10" />
            <img src={home} alt="Home" onClick={() => { navigate("/") }} className="cursor-pointer h-10 w-10" />
          </div>
        </div>

        {/* Projects Table */}
        <div className="bg-white rounded-xl mt-3 border border-gray-300">
          <div className="bg-white rounded-2xl overflow-hidden">

            {/* Header */}
            <div className="px-4 py-3 border-b border-gray-300 flex justify-between items-center">
              <h2 className="text-2xl  font-bold text-gray-700">
                Projects
              </h2>

              <button className="bg-[#1A1953] hover:bg-[#141343] text-white px-4 py-2 rounded-lg font-medium text-lg">
                Create Project
              </button>
            </div>

            {/* Table */}
            <table className="w-full text-left">
              <thead className="bg-blue-100 text-gray-600 text-lg uppercase">
                <tr>
                  <th className="px-6 py-3">Project</th>
                  <th className="px-6 py-3">Status</th>
                  <th className="px-6 py-3">Members</th>
                  <th className="px-6 py-3 text-center">Actions</th>
                </tr>
              </thead>

              <tbody>
                {allProjects.length > 0 ? allProjects.map((project) => (
                  <tr key={project.id} className="">
                    <td className="px-6 py-2">{project.projectName}</td>
                    <td className="px-6 py-2">{project.projectStatus}</td>
                    <td className="px-6 py-2">
                      {project.projectMembers?.join(", ") || "No members"}
                    </td>
                    <td className="px-6 py-2 text-center">
                      <img src={dlt} alt="Delete" className="m-auto w-7 h-7 cursor-pointer" />
                    </td>
                  </tr>
                )) : (
                  <tr>
                    <td colSpan={4} className="px-6 py-4 text-center text-gray-500">
                      No projects found.
                    </td>
                  </tr>
                )}

              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  )
}

export default AdminDashBoard;