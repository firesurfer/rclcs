cmake_minimum_required(VERSION 3.5)

project(rclcs C)



find_package(ament_cmake REQUIRED)
find_package(ament_cmake_python REQUIRED)

find_package(dotnet_cmake_module REQUIRED)
find_package(DotNETExtra MODULE)



add_msbuild(${PROJECT_NAME}
	CSPROJ
	${CMAKE_CURRENT_SOURCE_DIR}/src/rclcs.csproj
)
install_assemblies(${PROJECT_NAME} DESTINATION lib)


ament_package(
  
)

