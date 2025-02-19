import type { NextConfig } from "next"

const nextConfig: NextConfig = {
    logging: {
        fetches: {
            fullUrl: true,
        },
    },
    images: {
        remotePatterns: [
            { protocol: "https", hostname: "cdn.pixabay.com" },
            { protocol: "https", hostname: "static.ybox.vn" },
            { protocol: "https", hostname: "encrypted-tbn0.gstatic.com" },
            { protocol: "https", hostname: "mms.img.susercontent.com" },
            { protocol: "https", hostname: "maycha.com.vn" }
        ],
    },
    output: "standalone",
}

export default nextConfig
