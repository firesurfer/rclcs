cmake_minimum_required(VERSION 3.5)

project(rclcs C)



find_package(ament_cmake REQUIRED)
find_package(ament_cmake_python REQUIRED)

find_package(dotnet_cmake_module REQUIRED)
find_package(DotNETExtra MODULE)
file(GLOB_RECURSE CS_SOURCES ${CMAKE_CURRENT_SOURCE_DIR}/src/*.cs)


add_assemblies("${PROJECT_NAME}_assemblies"
  ${CS_SOURCES}
  OUTPUT_NAME
  "${PROJECT_NAME}"
  COMPILER_ARGS
  "-unsafe"
  "-doc:rclcs.xml"
)
install_assemblies("${PROJECT_NAME}_assemblies" COPY_DEPENDENCIES DESTINATION lib)
#if(NOT WIN32)
#add_custom_target(rclcs ALL
#	COMMAND 
#	"xbuild" rclcs.csproj /property:OutputPath=${CMAKE_BINARY_DIR} /property:AssemblyName=rclcs /property:GenerateDocumentation=true /property:DocumentationFile="rclcs.xml"
#	WORKING_DIRECTORY
#	${CMAKE_SOURCE_DIR}/src
#)
#else()
#add_custom_target(rclcs ALL
#	COMMAND 
#	"msbuild" rclcs.csproj /property:OutputPath=${CMAKE_BINARY_DIR} /property:AssemblyName=rclcs /property:Platform=x64 /property:AllowUnsafeBlocks=true /property:GenerateDocumentation=true /property:DocumentationFile="rclcs.xml"
#	WORKING_DIRECTORY
#	${CMAKE_SOURCE_DIR}/src
#)
#endif()

#ament_export_dependencies(rosidl_cmake)
#ament_export_include_directories(include)
#ament_export_libraries(${PROJECT_NAME})

#ament_python_install_package(${PROJECT_NAME})



ament_package(
  
)
#if(NOT WIN32)
#	install(FILES ${CMAKE_BINARY_DIR}/rclcs.dll DESTINATION lib)
#	install(FILES ${CMAKE_CURRENT_SOURCE_DIR}/src/rclcs.xml DESTINATION lib)
#else()
#	install(FILES ${CMAKE_BINARY_DIR}/rclcs.dll DESTINATION bin)
#	install(FILES ${CMAKE_CURRENT_SOURCE_DIR}/src/rclcs.xml DESTINATION bin)
#endif()

#install(TARGETS rclcs.dll DESTINATION lib)
#install(
#  PROGRAMS bin/rosidl_generator_cs
#  DESTINATION lib/rosidl_generator_cs
#)


