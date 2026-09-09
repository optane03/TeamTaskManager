import { useQuery } from "@tanstack/react-query"
import axiosRequest from "../Utils/AxiosRequest"
import { useEffect } from "react"

interface Project {
    id: string,
    projectName: string,
    useEmail: string,
    projectStatus: string
}

const Projects = () => {
    const GetProjects = async () => {
        try {
            const result = await axiosRequest.get(
                "Project/GetAllProjectDetails"
            );

            console.log(result.data);
        } catch (error) {
            console.log(error);
        }
    }

    useEffect(() => {
        GetProjects()
    }, [])

    return (
        <>

        </>
    )
}

export default Projects