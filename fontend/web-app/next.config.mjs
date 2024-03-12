// @ts-check

// export default (phase, { defaultConfig }) => {
//   /**
//    * @type {import('next').NextConfig}
//    */
//   const nextConfig = {
//     experimental: {
//       serverActions: true,
//     },
//     images: {
//       domains: ["cdn.pixabay.com"],
//     },
//   };
//   return nextConfig;
// };

// @ts-check

// @ts-check

/**
 * @type {import('next').NextConfig}
 */
const nextConfig = {
  experimental: {
    serverActions: true,
  },
  images: {
    domains: ["cdn.pixabay.com"],
  },
};

export default nextConfig;
