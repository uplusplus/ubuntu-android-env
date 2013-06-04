LOCAL_PATH := $(call my-dir)
include $(CLEAR_VARS)
LOCAL_MODULE_TAGS := optional

LOCAL_MODULE := sickld

LOCAL_CFLAGS += -fexceptions -frtti
LOCAL_CPPFLAGS += -fexceptions -frtti

LOCAL_LDFLAGS += $(LOCAL_PATH)/prebuild/libstdc++.a -Wl,-v 

LOCAL_SHARED_LIBRARIES := libcutils libutils libstlport libstdc++

LOCAL_C_INCLUDES := \
    $(LOCAL_PATH) \
    external/stlport/stlport \
    bionic

LOCAL_SRC_FILES := exception.cpp


include $(BUILD_EXECUTABLE)
