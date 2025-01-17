# syntax=docker/dockerfile:1

# Use a multi-stage build to reduce the size of the final image
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

# Copy source code into the build container
COPY . /source
WORKDIR /source/ScheduleApp.Server

# Build the application
ARG TARGETARCH
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish -a ${TARGETARCH/amd64/x64} --use-current-runtime --self-contained false -o /app

# Use a runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

# Copy the application from the build stage
COPY --from=build /app .

# Set up the environment variable for the Firebase key
ENV GOOGLE_APPLICATION_CREDENTIALS=/app/serviceAccountKey.json

# Switch to a non-privileged user
USER $APP_UID

# Run the application
ENTRYPOINT ["dotnet", "ScheduleApp.Server.dll"]
